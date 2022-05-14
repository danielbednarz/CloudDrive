import { defineStore } from "pinia";
import { api } from "src/boot/axios";
import { useAuthenticationStore } from "./authentication";
const authenticationStore = useAuthenticationStore();

export const useFileStore = defineStore({
  id: "file",
  state: () => ({}),
  actions: {
    getUserFiles() {
      return new Promise((resolve) => {
        api
          .get("/File/getUserFiles", {
            headers: {
              Authorization: `Bearer ${authenticationStore.currentUser.token}`,
            },
          })
          .then((response) => {
            this.files = response.data;
            resolve(response.data);
          });
      });
    },
  },
});
