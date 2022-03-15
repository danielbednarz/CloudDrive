import { defineStore } from "pinia";

export const useMainStore = defineStore({
  id: "main",
  state: () => ({
    user: "Administrator",
  }),
  getters: {
    getLoggedInUser() {
      return this.user;
    },
  },
});
