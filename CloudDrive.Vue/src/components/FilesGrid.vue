<template>
  <div class="section-container">
    <p class="text-h5 q-mt-lg q-mb-xs">Pliki</p>
    <div class="q-pa-md">
      <q-table
        grid
        :rows="files"
        :columns="columns"
        row-key="id"
        v-if="files"
        no-data-label="Brak plików"
        no-results-label="Brak danych"
        loading-label="Ładowanie..."
        rows-per-page-label="Wielkość strony"
      >
        <template v-slot:item="props">
          <div class="q-pa-xs col-2">
            <q-card class="file-card bg-secondary text-black">
              <q-card-actions class="delete-button" text-align="right">
                <q-btn
                  flat
                  round
                  icon="fa-solid fa-trash"
                  @click="tryDelete(props.row.relativePath)"
                />
              </q-card-actions>
              <q-card-section class="text-center">
                <i class="fa-solid fa-file text-h5"></i>
                <div class="text-body2">{{ props.row.name }}</div>
              </q-card-section>
              <q-separator />
              <q-card-section
                class="flex flex-center"
                :style="{ fontSize: 12 + props.row.size / 250000 + 'px' }"
              >
                <div>{{ (props.row.size / 1000000).toFixed(2) }} MB</div>
              </q-card-section>
            </q-card>
          </div>
        </template>
        <!-- <template v-slot:top-right>
        <q-input
          borderless
          dense
          debounce="300"
          v-model="filter"
          placeholder="Search"
        >
          <template v-slot:append>
            <q-icon name="search" />
          </template>
        </q-input>
      </template> -->
      </q-table>
    </div>
  </div>
</template>

<script>
import { useDirectoryStore } from "../stores/directory.js";
import { useFileStore } from "../stores/file.js";
import { mapActions } from "pinia";

export default {
  name: "FileGrid",
  data() {
    return {
      directories: [],
      files: [],
      columns: [
        {
          name: "Name",
          field: (row) => row.name,
          format: (val) => `${val}`,
        },
        {
          name: "Size",
          field: (row) => row.size,
          format: (val) => `${val}`,
        },
      ],
    };
  },
  computed: {},
  methods: {
    ...mapActions(useDirectoryStore, ["getDirectoriesToSelectList"]),
    ...mapActions(useFileStore, ["getUserFiles", "deleteFile"]),
    async tryDelete(relativePath) {
      await this.deleteFile(relativePath);
      this.getUserFiles().then((response) => {
        this.files = response;
        this.$q.notify({
          type: "positive",
          message: `Plik usunięty pomyślnie!`,
        });
      });
    },
  },
  mounted() {
    this.getUserFiles().then((response) => {
      this.files = response;
    });
    this.getDirectoriesToSelectList().then((response) => {
      this.directories = response;
    });
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
</style>
