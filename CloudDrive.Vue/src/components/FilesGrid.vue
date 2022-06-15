<template>
  <div class="section-container">
    <div class="row q-mt-lg q-mx-md">
      <div class="col-3">
        <q-btn
          label="Pobierz zaznaczone pliki"
          icon="fa-solid fa-list-check"
          color="primary"
          text-color="white"
          type="submit"
          :disabled="ticked.length === 0"
          @click="tryDownloadTickedFiles"
        />
      </div>
    </div>
    <div class="row q-mt-sm q-mx-md">
      <div class="col-3">
        <q-checkbox
          v-model="showDeleted"
          label="Pokaż usunięte pliki"
          class="float-left"
          @update:model-value="onShowDeletedCheckboxUpdate"
        />
      </div>
      <div class="col-6">
        <p class="text-h5 q-mt-lg q-mb-xs">Pliki</p>
      </div>
    </div>
    <div class="q-pa-md text-white">
      <q-splitter v-model="splitterModel">
        <template v-slot:before>
          <q-tree
            :nodes="getUserDirectories"
            node-key="id"
            color="secondary"
            label-key="name"
            dark
            v-model:ticked="ticked"
            v-model:selected="selected"
            ref="directoriesTree"
            no-nodes-label="Dysk jest pusty"
            no-results-label="Dysk jest pusty"
            tick-strategy="strict"
          />
        </template>
        <template v-slot:after>
          <p class="text-body1" v-if="!selected">Wybierz plik lub folder</p>
          <div v-else>
            <p class="text-body1">Wybrano {{ getSelectedObject.name }}</p>
            <p class="text-body2">
              Jego ścieżka to
              {{ getSelectedObject.relativePath }}
            </p>
            <div
              class="row justify-center q-my-xl"
              v-if="getSelectedObject.isFile"
            >
              <div class="column">
                <q-btn
                  icon="fa-solid fa-download"
                  color="secondary"
                  text-color="black"
                  label="Pobierz"
                  class="q-mx-sm"
                  @click="tryDownload()"
                />
              </div>
              <div class="column">
                <q-btn
                  icon="fa-solid fa-trash"
                  color="secondary"
                  text-color="black"
                  label="Usun"
                  class="q-mx-sm"
                  @click="tryDelete()"
                  v-if="!showDeleted"
                />
              </div>
              <div class="column">
                <q-btn
                  icon="fa-solid fa-code-branch"
                  color="secondary"
                  text-color="black"
                  label="Wersje"
                  class="q-mx-sm"
                  @click="isVersionsDialogVisible = true"
                />
              </div>
            </div>
            <div
              class="row justify-center q-my-xl"
              v-if="!getSelectedObject.isFile"
            >
              <div class="column">
                <q-btn
                  icon="fa-solid fa-file-zipper"
                  color="secondary"
                  text-color="black"
                  label="Pobierz"
                  class="q-mx-sm"
                  @click="tryDownloadDirectory()"
                />
              </div>
            </div>
            <file-versions-dialog
              :isVersionsDialogVisible="isVersionsDialogVisible"
              :fileId="getSelectedObject.id"
              @dialog-hide="isVersionsDialogVisible = false"
              @dialog-hide-selected="onDialogHideSelected"
              v-if="getSelectedObject.isFile"
            />
          </div>
        </template>
      </q-splitter>
      <q-inner-loading :showing="isLoading">
        <q-spinner size="50px" color="primary" />
      </q-inner-loading>
    </div>
  </div>
</template>

<script>
import { useDirectoryStore } from "../stores/directory.js";
import { useFileStore } from "../stores/file.js";
import { mapActions } from "pinia";
import FileVersionsDialog from "./FileVersionsDialog";

export default {
  name: "FileGrid",
  data() {
    return {
      userDirectories: [],
      selected: null,
      ticked: [],
      splitterModel: 50,
      isVersionsDialogVisible: false,
      showDeleted: false,
      isLoading: false,
    };
  },
  computed: {
    getSelectedObject() {
      return this.selected
        ? this.$refs["directoriesTree"].getNodeByKey(this.selected)
        : null;
    },
    getUserDirectories() {
      return this.userDirectories;
    },
  },
  methods: {
    ...mapActions(useDirectoryStore, [
      "getDirectoriesToSelectList",
      "getUserDriveDataToTreeView",
      "downloadDirectory",
    ]),
    ...mapActions(useFileStore, [
      "getUserFiles",
      "deleteFile",
      "downloadFile",
      "downloadTickedFiles",
    ]),
    async tryDelete() {
      await this.deleteFile(this.getSelectedObject.relativePath);
      await this.loadUserDriveData();
      this.$q.notify({
        type: "positive",
        message: `Plik usunięty pomyślnie!`,
      });
    },
    async tryDownload() {
      await this.downloadFile({
        id: this.getSelectedObject.id,
        name: this.getSelectedObject.name,
      });
    },
    async tryDownloadDirectory() {
      await this.downloadDirectory({
        id: this.getSelectedObject.id,
        name: this.getSelectedObject.name,
      });
    },
    async onDialogHideSelected() {
      this.isVersionsDialogVisible = false;
      await this.loadUserDriveData();
      this.$q.notify({
        type: "positive",
        message: `Wersja wybrana pomyślnie!`,
      });
    },
    async loadUserDriveData() {
      this.isLoading = true;
      this.selected = null;
      await this.getUserDriveDataToTreeView(this.showDeleted).then(
        (response) => {
          this.userDirectories = response;
          this.isLoading = false;
        }
      );
    },
    async tryDownloadTickedFiles() {
      await this.downloadTickedFiles(this.ticked);
    },
    onShowDeletedCheckboxUpdate(val, e) {
      this.loadUserDriveData();
    },
  },
  mounted() {
    // this.getUserFiles().then((response) => {
    //   this.files = response;
    // });
    this.loadUserDriveData();
  },
  components: {
    FileVersionsDialog,
  },
};
</script>
<style lang="scss" scoped>
.file-card {
  opacity: 0.8;
  transition: 0.3s;
}
.file-card:hover {
  opacity: 1;
  cursor: pointer;
}
.delete-button {
  padding: 0;
  margin-left: auto;
}
.small-icon {
  font-size: 0.7em;
}
</style>
