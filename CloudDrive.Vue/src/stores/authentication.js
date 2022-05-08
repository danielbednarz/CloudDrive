import { defineStore } from "pinia";
import { api } from "src/boot/axios";

export const useAuthenticationStore = defineStore({
  id: "authentication",
  state: () => ({
    form: {
      Username: "",
      Password: "",
    },
    currentUser: {
      username: "",
      token: "",
    },
  }),
  actions: {
    async login() {
      await api.post("/Users/login", this.form).then((response) => {
        this.currentUser.username = response.data.username;
        this.currentUser.token = response.data.token;
        localStorage.setItem("user", JSON.stringify(response.data));
      });
    },
    clearForm() {
      this.form = [];
    },
  },
});
