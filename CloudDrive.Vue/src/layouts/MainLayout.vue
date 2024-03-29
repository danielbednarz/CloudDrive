<template>
  <q-layout view="lHh Lpr lFf">
    <q-header bordered>
      <q-toolbar>
        <q-btn
          flat
          dense
          round
          :icon="getIcon()"
          aria-label="Menu"
          @click="toggleLeftDrawer"
        />

        <q-toolbar-title
          ><i class="fa-solid fa-cloud q-mx-xs"></i> CloudDrive
        </q-toolbar-title>
        <div v-if="currentUser.token">
          Witaj, {{ currentUser.username }}!
          <q-btn
            flat
            round
            icon="fa-solid fa-arrow-right-from-bracket"
            aria-label="Wyloguj się"
            @click="tryLogout"
          />
        </div>
        <div v-else>
          <q-btn
            flat
            round
            icon="fa-solid fa-arrow-right-to-bracket"
            aria-label="Zaloguj się"
            @click="showLoginPopover"
          />
        </div>
      </q-toolbar>
    </q-header>

    <q-drawer v-model="leftDrawerOpen" show-if-above bordered>
      <q-list>
        <q-item-label header> Menu aplikacji </q-item-label>

        <EssentialLink
          v-for="link in essentialLinks"
          :key="link.title"
          v-bind="link"
        />
      </q-list>
    </q-drawer>

    <q-page-container>
      <login-form
        v-if="isLoginPopoverVisible"
        :isLoginPopoverVisibleProp="isLoginPopoverVisible"
        @closePopover="hideLoginPopover"
      />
      <router-view v-if="isLoaded" />
    </q-page-container>
  </q-layout>
</template>

<script>
import EssentialLink from "components/EssentialLink.vue";
import LoginForm from "../components/LoginForm";

const linksList = [
  {
    title: "Strona główna",
    caption: "Przejdź na stronę główną",
    icon: "fa-solid fa-house",
    internalPathName: "home",
    isTargetBlank: false,
  },
  {
    title: "Mój dysk",
    caption: "Sprawdź zawartość swojego dysku",
    icon: "fa-solid fa-hard-drive",
    internalPathName: "drive",
    isTargetBlank: false,
  },
  {
    title: "Dodaj plik",
    caption: "Dodaj nową zawartość do dysku",
    icon: "fa-solid fa-file-arrow-up",
    internalPathName: "upload",
    isTargetBlank: false,
    separator: true,
  },
  {
    title: "Repozytorium",
    caption: "Repozytorium na platformie GitHub",
    icon: "fa-brands fa-github",
    link: "https://github.com/danielbednarz/CloudDrive",
    isTargetBlank: true,
  },
];

import { mapActions, mapWritableState } from "pinia";
import { useAuthenticationStore } from "../stores/authentication.js";
import { connectToHub } from "../hubs/file-hub";

export default {
  name: "MainLayout",
  components: {
    EssentialLink,
    LoginForm,
  },
  data() {
    return {
      leftDrawerOpen: true,
      rightDrawerOpen: false,
      essentialLinks: linksList,
      isLoginPopoverVisible: false,
      isLoaded: false,
    };
  },
  computed: {
    ...mapWritableState(useAuthenticationStore, [
      "currentUser",
      "signalrConnection",
    ]),
  },
  methods: {
    ...mapActions(useAuthenticationStore, ["logout"]),
    toggleLeftDrawer() {
      this.leftDrawerOpen = !this.leftDrawerOpen;
    },
    getIcon() {
      return this.leftDrawerOpen
        ? "fa-solid fa-square-caret-left"
        : "fa-solid fa-ellipsis";
    },
    showLoginPopover() {
      this.isLoginPopoverVisible = true;
    },
    hideLoginPopover() {
      this.isLoginPopoverVisible = false;
    },
    tryLogout() {
      this.logout();
      this.$q.notify({
        type: "info",
        message: `Wylogowano pomyślnie`,
      });
      this.$router.push({ name: "home" });
    },
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
        this.isLoaded = true;
      });
    } else {
      this.$api.get("/Users/checkConnection").then(() => {
        this.$q.loading.hide();
        this.isLoaded = true;
      });
    }
  },
};
</script>
