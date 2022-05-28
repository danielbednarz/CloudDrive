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

    async downloadFile({ id, name }) {
      if (
        authenticationStore.currentUser &&
        authenticationStore.currentUser.token
      ) {
        await api
          .get("/File/downloadFile", {
            responseType: "blob",
            headers: {
              Authorization: `Bearer ${authenticationStore.currentUser.token}`,
            },
            params: {
              fileId: id,
            },
          })
          .then((response) => {
            const blob = new Blob([response.data], {
              type: response.data.type,
            });
            const link = document.createElement("a");
            link.href = URL.createObjectURL(blob);
            link.download = name;
            link.click();
            URL.revokeObjectURL(link.href);
          });
      }
    },

    getFileVersions(fileId) {
      return new Promise((resolve, reject) => {
        api
          .get("/File/getFileVersions", {
            headers: {
              Authorization: `Bearer ${authenticationStore.currentUser.token}`,
            },
            params: {
              fileId: fileId,
            },
          })
          .then((response) => {
            resolve(response);
          });
      });
    },
  },
});
