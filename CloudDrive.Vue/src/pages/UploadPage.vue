<template>
  <transition
    appear
    enter-active-class="animated fadeIn"
    leave-active-class="animated fadeOut"
  >
    <q-page class="flex justify-center">
      <FileUploader />
    </q-page>
  </transition>
</template>

<script>
import FileUploader from "../components/FileUploader.vue";

export default {
  name: "UploadPage",
  components: {
    FileUploader,
  },
};
</script>
