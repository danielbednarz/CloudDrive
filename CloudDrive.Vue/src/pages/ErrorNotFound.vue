<template>
  <div
    class="fullscreen bg-red text-white text-center q-pa-md flex flex-center"
  >
    <div>
      <div style="font-size: 30vh">404</div>

      <div class="text-h2" style="opacity: 0.4">
        Nie znaleziono wybranej strony.
      </div>

      <q-btn
        class="q-mt-xl"
        color="white"
        text-color="red"
        unelevated
        to="/"
        label="Powrot na strone glowna"
      />
    </div>
  </div>
</template>

<script>
export default {
  name: "ErrorNotFound",
};
</script>
