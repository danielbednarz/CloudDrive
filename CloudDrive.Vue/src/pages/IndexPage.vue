<template>
  <q-page class="flex flex-center">
    <div class="q-pa-sm">
      <q-responsive :ratio="16 / 9" class="message-style">
        <div class="rounded-borders bg-primary text-white text-center">
          <div class="row">
            <p class="text-h2 full-width text-center q-mt-xl">
              Witamy w aplikacji CloudDrive
            </p>
          </div>
          <div class="row">
            <p class="text-subtitle1 full-width text-center">
              miejscu, gdzie wygodnie i bezpiecznie przechowasz swoje pliki.
            </p>
          </div>
          <div class="row full-width text-center q-mt-xl">
            <div class="col-3 offset-md-3">
              <q-btn
                color="secondary"
                text-color="black"
                label="Zaloguj sie"
                class="q-mr-md"
              />
            </div>
            <div class="col-3">
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
</template>

<script>
export default {
  name: "IndexPage",
};
</script>
<style scoped>
.message-style {
  width: 40vw;
}
.text-subtitle1 {
  opacity: 0.6;
}
</style>
