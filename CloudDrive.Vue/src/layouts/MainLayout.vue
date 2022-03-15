<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated>
      <q-toolbar>
        <q-btn
          flat
          dense
          round
          icon="menu"
          aria-label="Menu"
          @click="toggleLeftDrawer"
        />

        <q-toolbar-title> CloudDrive </q-toolbar-title>

        <div>Zalogowano jako: {{ user }}</div>
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
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script>
import EssentialLink from "components/EssentialLink.vue";
import { useMainStore } from "../stores/main.js";
import { mapState } from "pinia";

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
    title: "Repozytorium",
    caption: "Repozytorium na platformie GitHub",
    icon: "fa-brands fa-github",
    link: "https://github.com/danielbednarz/CloudDrive",
    isTargetBlank: true,
  },
];

export default {
  name: "MainLayout",
  computed: {
    ...mapState(useMainStore, ["user"]),
  },
  components: {
    EssentialLink,
  },
  data() {
    return {
      leftDrawerOpen: false,
      essentialLinks: linksList,
    };
  },
  methods: {
    toggleLeftDrawer() {
      this.leftDrawerOpen = !this.leftDrawerOpen;
    },
  },
};
</script>
