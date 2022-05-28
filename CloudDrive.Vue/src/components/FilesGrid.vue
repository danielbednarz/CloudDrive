<template>
  <div class="section-container">
    <p class="text-h5 q-mt-lg q-mb-xs">Pliki</p>
    <div class="q-pa-md text-white">
      <q-splitter v-model="splitterModel">
        <template v-slot:before>
          <q-tree
            :nodes="getUserDirectories"
            node-key="id"
            color="secondary"
            label-key="name"
            dark
            v-model:selected="selected"
            ref="directoriesTree"
            no-nodes-label="Dysk jest pusty"
            no-results-label="Dysk jest pusty"
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
            <file-versions-dialog
              :isVersionsDialogVisible="isVersionsDialogVisible"
              :fileId="getSelectedObject.id"
              @dialog-hide="isVersionsDialogVisible = false"
              v-if="getSelectedObject.isFile"
            />
          </div>
        </template>
      </q-splitter>
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
      splitterModel: 50,
      isVersionsDialogVisible: false,
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
    ]),
    ...mapActions(useFileStore, ["getUserFiles", "deleteFile", "downloadFile"]),
    async tryDelete() {
      await this.deleteFile(this.getSelectedObject.relativePath);
      this.getUserDriveDataToTreeView().then((response) => {
        this.userDirectories = response;
        this.selected = null;
        this.$q.notify({
          type: "positive",
          message: `Plik usunięty pomyślnie!`,
        });
      });
    },
    async tryDownload() {
      await this.downloadFile({
        id: this.getSelectedObject.id,
        name: this.getSelectedObject.name,
      });
    },
  },
  mounted() {
    // this.getUserFiles().then((response) => {
    //   this.files = response;
    // });
    this.getUserDriveDataToTreeView().then((response) => {
      this.userDirectories = response;
    });
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
