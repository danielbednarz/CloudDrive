<template>
  <q-dialog v-model="carousel" @hide="onDialogHide" v-if="fileVersions">
    <q-carousel
      transition-prev="slide-right"
      transition-next="slide-left"
      swipeable
      animated
      v-model="slide"
      control-color="primary"
      navigation-icon="fa-solid fa-circle-dot"
      navigation
      padding
      height="42vh"
      class="bg-secondary shadow-1 rounded-borders"
    >
      <q-carousel-slide
        :name="index"
        class="column no-wrap flex-center"
        v-for="(version, index) in fileVersions"
        :key="index"
      >
        <q-icon
          :name="version.isDeleted ? 'fa-solid fa-xmark' : 'fa-solid fa-check'"
          color="primary"
          size="36px"
          class="q-my-md"
        />
        <div class="text-center text-black">
          <p class="text-body2">
            {{ version.isDeleted ? "Wersja nieaktualna" : "Wersja aktualna" }}
          </p>
          <p class="q-mt-lg text-h5">{{ version.name }}</p>
          <p class="text-body2">
            To jest wersja numer {{ version.fileVersion + 1 }}
          </p>
          <p class="text-body2">
            Jej rozmiar wynosi {{ version.size / 1000000 }} MB
          </p>
          <p class="text-body2">
            Utworzona
            {{ new Date(version.createdDate).toLocaleString("pl-PL") }}
          </p>
          <q-btn
            color="primary"
            text-color="secondary"
            label="Wybierz"
            class="q-my-sm"
            @click="selectVersion(version.id)"
          />
        </div>
      </q-carousel-slide>
    </q-carousel>
  </q-dialog>
</template>

<script>
import { mapActions } from "pinia";
import { useFileStore } from "../stores/file.js";
import { ref } from "vue";

export default {
  name: "FileVersionsDialog",
  props: {
    isVersionsDialogVisible: {
      type: Boolean,
      default: false,
    },
    fileId: {
      type: String,
      required: true,
    },
  },
  data() {
    return {
      carousel: ref(false),
      slide: ref(0),
      fileVersions: [],
    };
  },
  watch: {
    isVersionsDialogVisible(newVal, oldVal) {
      this.carousel = newVal;
      this.getFileVersions(this.fileId).then((response) => {
        this.fileVersions = response.data;
      });
    },
  },
  methods: {
    ...mapActions(useFileStore, ["getFileVersions", "selectFileVersion"]),
    onDialogHide() {
      this.$emit("dialog-hide");
      this.fileVersions = [];
    },
    selectVersion(id) {
      this.selectFileVersion(id).then(() => {
        this.$emit("dialog-hide-selected");
        this.fileVersions = [];
      });
    },
  },
  mounted() {
    this.getFileVersions(this.fileId).then((response) => {
      this.fileVersions = response.data;
    });
  },
};
</script>
