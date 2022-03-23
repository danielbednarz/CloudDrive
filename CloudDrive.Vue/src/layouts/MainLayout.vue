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

        <div>Witaj, {{ user }}!</div>
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
      leftDrawerOpen: true,
      rightDrawerOpen: false,
      essentialLinks: linksList,
    };
  },
  methods: {
    toggleLeftDrawer() {
      this.leftDrawerOpen = !this.leftDrawerOpen;
    },
    getIcon() {
      return this.leftDrawerOpen
        ? "fa-solid fa-square-caret-left"
        : "fa-solid fa-ellipsis";
    },
  },
};
</script>
