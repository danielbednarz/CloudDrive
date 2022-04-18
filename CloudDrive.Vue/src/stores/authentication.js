import { defineStore } from "pinia";
import { api } from "src/boot/axios";

export const useAuthenticationStore = defineStore({
  id: "authentication",
  state: () => ({
    form: {
      Username: "",
      Password: "",
    },
    user: {
      username: "",
      token: "",
    },
  }),
  actions: {
    async login() {
      await api.post("/Users/login", this.form).then((response) => {
        this.user = response.data;
      });
    },
    clearForm() {
      this.form = [];
    },
  },
});
