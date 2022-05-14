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

    async deleteFile(relativePath) {
      if (
        authenticationStore.currentUser &&
        authenticationStore.currentUser.token
      ) {
        await api.delete("/File/deleteFile", {
          headers: {
            Authorization: `Bearer ${authenticationStore.currentUser.token}`,
          },
          params: {
            relativePath: relativePath,
          },
        });
      }
    },
  },
});
