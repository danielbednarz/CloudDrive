import { defineStore } from "pinia";

Vue.prototype.$currentUser = '';

export const useMainStore = defineStore({
  id: "main",
});
