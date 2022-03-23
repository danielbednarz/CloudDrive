<template>
  <transition
    appear
    enter-active-class="animated fadeIn"
    leave-active-class="animated fadeOut"
  >
    <q-page class="flex justify-center">
      <main-container title="CloudDrive">
        <div class="row">
          <p class="text-subtitle1 full-width text-center">
            Miejsce, gdzie wygodnie i bezpiecznie przechowasz swoje pliki.
          </p>
        </div>
        <div class="row full-width text-center q-mt-xl justify-center">
          <div>
            <q-btn
              color="secondary"
              text-color="black"
              label="Logowanie"
              class="q-mr-md"
            />
          </div>
          <div>
            <q-btn
              color="secondary"
              text-color="black"
              label="Rejestracja"
              class="q-ml-md"
            />
          </div>
        </div>
      </main-container>
    </q-page>
  </transition>
</template>

<script>
import MainContainer from "../components/MainContainer";

export default {
  name: "IndexPage",
  components: {
    MainContainer,
  },
};
</script>
<style scoped>
.text-subtitle1 {
  opacity: 0.6;
}
</style>
