<template>
  <transition
    appear
    enter-active-class="animated fadeIn"
    leave-active-class="animated fadeOut"
  >
    <q-page class="flex justify-center">
      <main-container title="Mój dysk">
        <div class="row">
          <q-space />
          <div class="column q-mx-md">
            <q-btn
              :label="getButtonLabel()"
              icon="fa-solid fa-folder-plus"
              color="secondary"
              :outline="isAddDirectoryMenuOpen"
              :text-color="getTextColor()"
              type="submit"
              @click="
                () => {
                  isAddDirectoryMenuOpen = !isAddDirectoryMenuOpen;
                }
              "
            />
          </div>
        </div>
        <transition
          appear
          enter-active-class="animated fadeIn"
          leave-active-class="animated fadeOut"
        >
          <directory-add-form
            v-if="isAddDirectoryMenuOpen"
            @close="onDirectoryAddFormClose"
            class="q-mt-md"
          />
        </transition>
        <div class="q-mt-md">
          <files-grid ref="filesGrid" />
        </div>
      </main-container>
    </q-page>
  </transition>
</template>

<script>
import MainContainer from "../components/MainContainer";
import DirectoryAddForm from "../components/DirectoryAddForm";
import FilesGrid from "../components/FilesGrid";
import { useAuthenticationStore } from "src/stores/authentication";
import { mapState } from "pinia";

export default {
  name: "DrivePage",
  data() {
    return {
      isAddDirectoryMenuOpen: false,
      filesGrid: null,
    };
  },
  computed: {
    ...mapState(useAuthenticationStore, ["signalrConnection"]),
    getFilesGrid() {
      return this.$refs["filesGrid"];
    },
  },
  components: {
    MainContainer,
    DirectoryAddForm,
    FilesGrid,
  },
  methods: {
    getButtonLabel() {
      return this.isAddDirectoryMenuOpen ? "Zamknij" : "Nowy folder";
    },
    getTextColor() {
      return this.isAddDirectoryMenuOpen ? "secondary" : "black";
    },
    onDirectoryAddFormClose() {
      this.isAddDirectoryMenuOpen = false;
      this.getFilesGrid.loadUserDriveData();
    },
  },
  mounted() {
    let context = this;
    this.signalrConnection.on("FileAdded", (id, fileName) => {
      context.getFilesGrid.loadUserDriveData();
    });
  },
};
</script>
