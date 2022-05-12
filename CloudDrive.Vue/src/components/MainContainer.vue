<template>
  <div class="q-pa-xl main-cont">
    <div
      class="rounded-borders bg-primary text-white text-center shadow-10 q-pa-xl"
    >
      <div class="row">
        <p class="text-h3 full-width text-center q-mt-xl">{{ title }}</p>
      </div>
      <q-separator class="q-mb-xl q-mt-sm" />
      <slot />
    </div>
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

<style lang="scss" scoped>
.main-cont {
  min-width: 80%;
  min-height: 50%;
}
</style>
