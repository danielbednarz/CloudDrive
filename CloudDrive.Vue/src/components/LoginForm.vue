<template>
  <q-dialog v-model="isLoginPopoverVisible" position="top">
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
          />
        </q-form>
      </q-card-section>
    </q-card>
  </q-dialog>
</template>

<script>
import { useAuthenticationStore } from "../stores/authentication.js";
import { mapActions, mapWritableState } from "pinia";

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
    };
  },
  computed: {
    ...mapWritableState(useAuthenticationStore, ["form"]),
  },
  methods: {
    ...mapActions(useAuthenticationStore, ["clearForm", "login"]),
    closePopover() {
      this.isLoginPopoverVisible = false;
      this.clearForm();
    },
    async tryLogin() {
      await this.login();
      this.closePopover();
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
