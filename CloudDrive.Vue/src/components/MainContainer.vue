<template>
  <div class="q-pa-sm">
    <q-responsive :ratio="16 / 9" class="big-container q-mt-xl">
      <div class="rounded-borders bg-primary text-white text-center shadow-10">
        <div class="row">
          <p class="text-h3 full-width text-center q-mt-xl">{{ title }}</p>
        </div>
        <slot />
      </div>
    </q-responsive>
  </div>
</template>

<script>
export default {
  name: "MainContainer",
  props: {
    title: {
      type: String,
      required: false,
      default: "",
    },
  },
};
</script>
