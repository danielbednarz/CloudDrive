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
          name="fa-solid fa-pen-to-square"
          color="primary"
          size="36px"
          class="q-my-md"
        />
        <div class="q-mt-md text-center text-black">
          <p class="text-h5">{{ version.name }}</p>
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
    ...mapActions(useFileStore, ["getFileVersions"]),
    onDialogHide() {
      this.$emit("dialog-hide");
      this.fileVersions = [];
    },
  },
  mounted() {
    this.getFileVersions(this.fileId).then((response) => {
      this.fileVersions = response.data;
    });
  },
};
</script>
