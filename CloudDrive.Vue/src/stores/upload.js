import { defineStore } from "pinia";

export const useUploadStore = defineStore({
  id: "upload",
  state: () => ({
    files: [],
  }),
  actions: {
    uploadFile(file) {
      debugger;
    },
    clearForm() {
      this.files = [];
    },
  },
});
