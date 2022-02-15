<template>
  <div>
    <div class="row">
      <div class="row header mt-3">
        <div class="col-2 h3 b mt-1">{{ title }}</div>
        <div class="col-8"></div>
        <div class="col-2">
          <div class="mt-2">
            <slot name="button"></slot>
            <br />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    title: {
      type: String,
      default: "Title",
    },
  },
};
</script>

<style scoped>
.header {
  border-bottom: 1px solid rgb(216, 216, 216);
  padding-bottom: 1rem;
  margin-bottom: 2rem;
}
</style>