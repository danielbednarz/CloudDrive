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
              :outline="isAddFolderMenuOpen"
              :text-color="getTextColor()"
              type="submit"
              @click="
                () => {
                  isAddFolderMenuOpen = !isAddFolderMenuOpen;
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
            v-if="isAddFolderMenuOpen"
            @close="
              () => {
                isAddFolderMenuOpen = false;
              }
            "
            class="q-mt-md"
          />
        </transition>
        <div class="q-mt-md">
          <files-grid />
        </div>
      </main-container>
    </q-page>
  </transition>
</template>

<script>
import MainContainer from "../components/MainContainer";
import DirectoryAddForm from "../components/DirectoryAddForm";
import FilesGrid from "../components/FilesGrid";

export default {
  name: "DrivePage",
  data() {
    return {
      isAddFolderMenuOpen: false,
    };
  },
  components: {
    MainContainer,
    DirectoryAddForm,
    FilesGrid,
  },
  methods: {
    getButtonLabel() {
      return this.isAddFolderMenuOpen ? "Zamknij" : "Dodaj folder";
    },
    getTextColor() {
      return this.isAddFolderMenuOpen ? "secondary" : "black";
    },
  },
};
</script>
