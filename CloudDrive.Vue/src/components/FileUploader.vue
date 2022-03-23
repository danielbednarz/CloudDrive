<template>
  <div class="q-pa-sm">
    <q-responsive :ratio="16 / 9" class="big-container q-mt-xl">
      <div class="rounded-borders bg-primary text-white text-center">
        <div class="row">
          <p class="text-h3 full-width text-center q-mt-xl">
            Dodawanie pliku do dysku
          </p>
        </div>
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
          />
        </div>
      </div>
    </q-responsive>
  </div>
</template>
<script>
import { useUploadStore } from "../stores/upload.js";
import { mapActions, mapWritableState } from "pinia";
import { useQuasar } from "quasar";

export default {
  name: "UploadPage",
  data() {
    return {
      $q: useQuasar(),
    };
  },
  computed: {
    ...mapWritableState(useUploadStore, ["files"]),
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
