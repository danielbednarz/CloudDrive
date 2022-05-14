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
    async register() {
      await api.post("/Users/register", this.form).then((response) => {
        this.currentUser.username = response.data.username;
        this.currentUser.token = response.data.token;
        localStorage.setItem("user", JSON.stringify(response.data));
      });
    },
    logout() {
      this.currentUser.username = "";
      this.currentUser.token = "";
      localStorage.removeItem("user");
    },
    clearForm() {
      this.form.Username = "";
      this.form.Password = "";
    },
  },
});
