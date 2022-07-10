<template>
  <q-dialog v-model="isLoginPopoverVisible" position="top" @hide="closePopover">
    <q-card class="container-login" bordered>
      <q-card-section class="row items-center no-wrap login-popover">
        <q-form class="q-gutter-md" @submit="tryLogin">
          <p class="text-h5 q-mt-lg q-mb-xs">Logowanie</p>
          <q-input
            filled
            v-model="form.Username"
            label="Login"
            hint="Podaj nazwę użytkownika"
            lazy-rules
            color="secondary"
            dark
            :rules="[
              (val) => (val && val.length > 0) || 'Login jest wymagany!',
            ]"
          />
          <q-input
            filled
            v-model="form.Password"
            label="Hasło"
            hint="Podaj hasło"
            lazy-rules
            color="secondary"
            dark
            type="password"
            :rules="[
              (val) => (val && val.length > 0) || 'Hasło jest wymagane!',
            ]"
          />

          <q-space />

          <q-btn
            label="Zaloguj"
            icon="fa-solid fa-arrow-right"
            text-color="secondary"
            outline
            type="submit"
            :loading="isLoading"
          />
        </q-form>
      </q-card-section>
    </q-card>
  </q-dialog>
</template>

<script>
import { useAuthenticationStore } from "../stores/authentication.js";
import { mapActions, mapState, mapWritableState } from "pinia";
import { connectToHub } from "../hubs/file-hub";

export default {
  name: "LoginForm",
  props: {
    isLoginPopoverVisibleProp: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      isLoginPopoverVisible: false,
      isLoading: false,
    };
  },
  computed: {
    ...mapWritableState(useAuthenticationStore, ["form", "signalrConnection"]),
    ...mapState(useAuthenticationStore, ["currentUser"]),
  },
  methods: {
    ...mapActions(useAuthenticationStore, ["clearForm", "login"]),
    closePopover() {
      this.isLoginPopoverVisible = false;
      this.clearForm();
      this.$emit("closePopover");
    },
    async tryLogin() {
      try {
        this.isLoading = true;
        await this.login();
        let connection = await connectToHub(this.currentUser.username, this.$q);
        this.signalrConnection = connection;
        this.$q.notify({
          type: "info",
          message: `Zalogowano pomyślnie`,
        });
      } catch (ex) {
        this.$q.notify({
          type: "negative",
          message: ex.response.data,
        });
      } finally {
        this.isLoading = false;
        this.closePopover();
      }
    },
  },
  mounted() {
    this.isLoginPopoverVisible = this.isLoginPopoverVisibleProp;
  },
  beforeUnmount() {
    this.closePopover();
  },
};
</script>
<style lang="scss" scoped>
.container-login {
  width: 350px;
  border-top: 4px solid $secondary;
  background-color: $dark;
}
</style>
