<template>
  <main-container title="Dodaj plik">
    <div class="row justify-center">
      <q-uploader
        url="https://localhost:44390/api/File/uploadFile"
        color="secondary"
        text-color="dark"
        square
        flat
        bordered
        class="uploader q-mt-lg"
        @added="added"
        @removed="clearForm"
        @uploading="onUploading"
        @uploaded="onUploaded"
        @failed="onError"
        :headers="[
          {
            name: 'Authorization',
            value: `Bearer ${this.currentUser.token}`,
          },
        ]"
      />
    </div>
  </main-container>
</template>
<script>
import { useUploadStore } from "../stores/upload.js";
import { mapActions, mapWritableState } from "pinia";
import { useQuasar } from "quasar";
import MainContainer from "./MainContainer";

export default {
  name: "UploadPage",
  data() {
    return {
      $q: useQuasar(),
      currentUser: [],
    };
  },
  computed: {
    ...mapWritableState(useUploadStore, ["files"]),
  },
  mounted() {
    this.currentUser = JSON.parse(localStorage.getItem("user"));
  },
  methods: {
    ...mapActions(useUploadStore, ["uploadFile", "clearForm"]),
    added(addedFiles) {
      this.files = addedFiles;
    },
    onUploading() {
      this.$q.notify({
        type: "info",
        spinner: true,
        message: "Dodaję plik...",
        timeout: 2500,
      });
    },
    onUploaded() {
      this.$q.notify({
        type: "positive",
        message: "Plik został dodany pomyślnie!",
      });
    },
    onError() {
      this.$q.notify({
        type: "negative",
        message: "Wystąpił błąd podczas dodawania pliku!",
      });
    },
  },
  components: {
    MainContainer,
  },
  beforeUnmount() {
    this.clearForm();
    this.files = [];
  },
};
</script>
<style scoped>
.uploader {
  width: 90%;
  min-width: 250px;
}
</style>
