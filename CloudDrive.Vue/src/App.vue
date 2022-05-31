<template>
  <router-view />
</template>

<script>
import { mapWritableState } from "pinia";
import { useAuthenticationStore } from "./stores/authentication.js";
import { connectToHub } from "./hubs/file-hub";

export default {
  name: "App",
  computed: {
    ...mapWritableState(useAuthenticationStore, [
      "currentUser",
      "signalrConnection",
    ]),
  },
  mounted() {
    if (localStorage.user) {
      let localStorageUser = JSON.parse(localStorage.user);
      this.currentUser.username = localStorageUser.username;
      this.currentUser.token = localStorageUser.token;

      let connection = connectToHub(localStorageUser.username, this.$q);
      this.signalrConnection = connection;
    }
  },
};
</script>
