import { Notify } from "quasar";

const routes = [
  {
    path: "/",
    component: () => import("layouts/MainLayout.vue"),
    children: [
      {
        path: "",
        name: "home",
        component: () => import("pages/IndexPage.vue"),
      },
      {
        path: "drive",
        name: "drive",
        beforeEnter: (to, from, next) => authGuard(to, from, next),
        component: () => import("pages/DrivePage.vue"),
      },
      {
        path: "upload",
        name: "upload",
        beforeEnter: (to, from, next) => authGuard(to, from, next),
        component: () => import("pages/UploadPage.vue"),
      },
    ],
  },

  {
    path: "/:catchAll(.*)*",
    component: () => import("pages/ErrorNotFound.vue"),
  },
];

function authGuard(to, from, next) {
  let isAuthenticated = false;
  if (localStorage.getItem("user")) {
    isAuthenticated = true;
  } else {
    isAuthenticated = false;
  }
  if (isAuthenticated) {
    next();
  } else {
    Notify.create({
      message: "Dostęp wzbroniony dla niezalogowanego użytkownika!",
      color: "negative",
    });
    next({ name: "home" });
  }
}

export default routes;
