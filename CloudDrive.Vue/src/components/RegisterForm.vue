<template>
  <q-dialog
    v-model="isRegisterPopoverVisible"
    position="top"
    @hide="closePopover"
  >
    <q-card class="container-register" bordered>
      <q-card-section class="row items-center no-wrap register-popover">
        <q-form class="q-gutter-md" @submit="tryRegister">
          <p class="text-h5 q-mt-lg q-mb-xs">Rejestracja</p>
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
          <q-input
            filled
            v-model="confirmPassword"
            label="Powtórz Hasło"
            hint="Podaj ponownie hasło"
            lazy-rules
            color="secondary"
            dark
            type="password"
            :rules="[
              (val) =>
                (val && val == form.Password) || 'Hasła muszą być identyczne!',
            ]"
          />

          <q-space />

          <q-btn
            label="Zarejestruj się"
            icon="fa-solid fa-arrow-right"
            text-color="secondary"
            outline
            type="submit"
          />
        </q-form>
      </q-card-section>
    </q-card>
  </q-dialog>
</template>

<script>
import { useAuthenticationStore } from "../stores/authentication.js";
import { mapActions, mapWritableState, mapState } from "pinia";
import { connectToHub } from "../hubs/file-hub";

export default {
  name: "RegisterForm",
  props: {
    isRegisterPopoverVisibleProp: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      isRegisterPopoverVisible: false,
      confirmPassword: "",
    };
  },
  computed: {
    ...mapWritableState(useAuthenticationStore, ["form"]),
    ...mapState(useAuthenticationStore, ["currentUser", "signalrConnection"]),
  },
  methods: {
    ...mapActions(useAuthenticationStore, ["clearForm", "register"]),
    closePopover() {
      this.isRegisterPopoverVisible = false;
      this.clearForm();
      this.$emit("closePopover");
    },
    async tryRegister() {
      await this.register();
      let connection = connectToHub(this.currentUser.username, this.$q);
      this.signalrConnection = connection;
      this.closePopover();
    },
  },
  mounted() {
    this.isRegisterPopoverVisible = this.isRegisterPopoverVisibleProp;
  },
  beforeUnmount() {
    this.closePopover();
  },
};
</script>
<style lang="scss" scoped>
.container-register {
  width: 350px;
  border-top: 4px solid $secondary;
  background-color: $dark;
}
</style>
