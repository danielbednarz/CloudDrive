<template>
  <transition
    appear
    enter-active-class="animated fadeIn"
    leave-active-class="animated fadeOut"
  >
    <q-page class="flex justify-center">
      <main-container title="Mój dysk"></main-container>
    </q-page>
  </transition>
</template>

<script>
import MainContainer from "../components/MainContainer";

export default {
  name: "DrivePage",
  components: {
    MainContainer,
  },
};
</script>
