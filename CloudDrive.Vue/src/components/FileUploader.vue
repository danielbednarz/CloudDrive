<template>
  <div class="justify-center" v-if="currentUser">
    <div class="row q-py-lg">
      <div class="column form-field">
        <q-select
          filled
          v-model="ChosenDirectory"
          label="Folder"
          color="secondary"
          dark
          :options="directories"
          option-label="text"
          option-value="value"
          emit-value
          map-options
          dense
          clearable
        />
      </div>
    </div>
    <div class="row">
      <q-uploader
        url="http://192.168.55.109:8005/api/File/uploadFile"
        color="secondary"
        text-color="dark"
        square
        flat
        bordered
        multiple
        class="uploader q-mt-lg"
        @removed="clearForm"
        @uploading="onUploading"
        @failed="onError"
        :headers="[
          {
            name: 'Authorization',
            value: `Bearer ${currentUser.token}`,
          },
        ]"
        :form-fields="[
          {
            name: 'DirectoryId',
            value: ChosenDirectory,
          },
        ]"
      />
    </div>
  </div>
</template>
<script>
import { useUploadStore } from "../stores/upload.js";
import { useAuthenticationStore } from "../stores/authentication.js";
import { useDirectoryStore } from "../stores/directory.js";
import { mapActions, mapWritableState, mapState } from "pinia";
import { useQuasar } from "quasar";

export default {
  name: "UploadPage",
  data() {
    return {
      directories: [],
      ChosenDirectory: "",
      $q: useQuasar(),
    };
  },
  computed: {
    //...mapWritableState(useUploadStore, ["files"]),
    ...mapState(useAuthenticationStore, ["currentUser"]),
  },
  methods: {
    ...mapActions(useUploadStore, ["uploadFile", "clearForm"]),
    ...mapActions(useDirectoryStore, ["getDirectoriesToSelectList"]),
    onUploading() {
      this.$q.notify({
        type: "info",
        spinner: true,
        message: "Dodaję plik...",
        timeout: 2000,
      });
    },
    onError() {
      this.$q.notify({
        type: "negative",
        message: "Wystąpił błąd podczas dodawania pliku!",
      });
    },
  },
  mounted() {
    this.getDirectoriesToSelectList().then((response) => {
      this.directories = response;
    });
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
.form-field {
  width: 40%;
}
</style>
