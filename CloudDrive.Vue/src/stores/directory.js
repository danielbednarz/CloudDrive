import { defineStore } from "pinia";
import { api } from "src/boot/axios";
import { useAuthenticationStore } from "./authentication";
const authenticationStore = useAuthenticationStore();

export const useDirectoryStore = defineStore({
  id: "directory",
  state: () => ({
    form: {
      Name: "",
      ParentDirectoryId: null,
    },
  }),
  actions: {
    async addDirectory() {
      await api.post("/File/addDirectory", this.form, {
        headers: {
          Authorization: `Bearer ${authenticationStore.currentUser.token}`,
        },
      });
    },
    clearForm() {
      this.form.Name = "";
      this.form.ParentDirectoryId = null;
    },
    getDirectoriesToSelectList() {
      if (
        authenticationStore.currentUser &&
        authenticationStore.currentUser.token
      ) {
        return new Promise((resolve) => {
          api
            .get("/File/getDirectoriesToSelectList", {
              headers: {
                Authorization: `Bearer ${authenticationStore.currentUser.token}`,
              },
            })
            .then((response) => {
              resolve(response.data);
            });
        });
      }
    },
    getUserDriveDataToTreeView() {
      if (
        authenticationStore.currentUser &&
        authenticationStore.currentUser.token
      ) {
        return new Promise((resolve) => {
          api
            .get("/File/getUserDriveDataToTreeView", {
              headers: {
                Authorization: `Bearer ${authenticationStore.currentUser.token}`,
              },
            })
            .then((response) => {
              resolve(response.data);
            });
        });
      }
    },
  },
});
