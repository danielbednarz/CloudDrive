<template>
  <div class="section-container">
    <p class="text-h5 q-mt-lg q-mb-xs">Dodawanie nowego folderu</p>
    <q-form class="q-gutter-md" @submit="tryAdd">
      <div class="row q-py-lg">
        <div class="column q-mx-md form-field">
          <q-input
            filled
            v-model="form.Name"
            label="Nazwa folderu"
            lazy-rules
            color="secondary"
            dark
            :rules="[
              (val) =>
                (val && val.length > 0) || 'Nazwa folderu jest wymagana!',
            ]"
            dense
          />
        </div>
        <div class="column q-mx-md form-field">
          <q-select
            filled
            v-model="form.ParentDirectoryId"
            label="Folder nadrzędny"
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
        <q-space />
        <div class="column q-mx-md">
          <q-btn label="Dodaj" text-color="secondary" outline type="submit" />
        </div>
      </div>
    </q-form>
  </div>
</template>

<script>
import { useDirectoryStore } from "../stores/directory.js";
import { mapActions, mapWritableState } from "pinia";

export default {
  name: "DirectoryAddForm",
  data() {
    return {
      directories: [],
    };
  },
  computed: {
    ...mapWritableState(useDirectoryStore, ["form"]),
  },
  methods: {
    ...mapActions(useDirectoryStore, [
      "clearForm",
      "addDirectory",
      "getDirectoriesToSelectList",
    ]),
    async tryAdd() {
      await this.addDirectory();
      this.$q.notify({
        type: "positive",
        message: "Folder został dodany pomyślnie!",
      });
      this.$emit("close");
    },
  },
  mounted() {
    this.getDirectoriesToSelectList().then((response) => {
      this.directories = response;
    });
  },
  beforeUnmount() {
    this.clearForm();
  },
};
</script>
<style lang="scss" scoped>
.form-field {
  width: 40%;
}
</style>
