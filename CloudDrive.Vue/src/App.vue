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
    this.$q.loading.show({
      message: "Nawiązuję połączenie z serwerem...",
    });
    if (localStorage.user) {
      let localStorageUser = JSON.parse(localStorage.user);
      this.currentUser.username = localStorageUser.username;
      this.currentUser.token = localStorageUser.token;

      connectToHub(localStorageUser.username, this.$q).then((response) => {
        this.$q.loading.hide();
        this.signalrConnection = response;
      });
    } else {
      this.$api.get("/Users/checkConnection").then(() => {
        this.$q.loading.hide();
      });
    }
  },
};
</script>
