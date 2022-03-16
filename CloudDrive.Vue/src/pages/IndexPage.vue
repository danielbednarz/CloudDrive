<template>
  <transition
    appear
    enter-active-class="animated fadeIn"
    leave-active-class="animated fadeOut"
  >
    <q-page class="flex justify-center">
      <div class="q-pa-sm">
        <q-responsive :ratio="16 / 9" class="big-container q-mt-xl">
          <div class="rounded-borders bg-primary text-white text-center">
            <div class="row">
              <p class="text-h3 full-width text-center q-mt-xl">
                Witamy w aplikacji CloudDrive
              </p>
            </div>
            <div class="row">
              <p class="text-subtitle1 full-width text-center">
                miejscu, gdzie wygodnie i bezpiecznie przechowasz swoje pliki.
              </p>
            </div>
            <div class="row full-width text-center q-mt-xl justify-center">
              <div>
                <q-btn
                  color="secondary"
                  text-color="black"
                  label="Zaloguj sie"
                  class="q-mr-md"
                />
              </div>
              <div>
                <q-btn
                  color="secondary"
                  text-color="black"
                  label="Zarejestruj sie"
                  class="q-ml-md"
                />
              </div>
            </div>
          </div>
        </q-responsive>
      </div>
    </q-page>
  </transition>
</template>

<script>
export default {
  name: "IndexPage",
};
</script>
<style scoped>
.text-subtitle1 {
  opacity: 0.6;
}
</style>
