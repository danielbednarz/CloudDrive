<template>
  <transition
    appear
    enter-active-class="animated fadeIn"
    leave-active-class="animated fadeOut"
  >
    <q-page class="flex justify-center">
      <main-container title="Dodaj plik">
        <FileUploader />
      </main-container>
    </q-page>
  </transition>
</template>

<script>
import FileUploader from "../components/FileUploader.vue";
import MainContainer from "../components/MainContainer";

export default {
  name: "UploadPage",
  components: {
    FileUploader,
    MainContainer,
  },
};
</script>
