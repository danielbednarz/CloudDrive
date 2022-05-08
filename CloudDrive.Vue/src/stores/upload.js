import { defineStore } from "pinia";

export const useUploadStore = defineStore({
  id: "upload",
  state: () => ({
    files: [],
  }),
  actions: {
    clearForm() {
      this.files = [];
    },
  },
});
