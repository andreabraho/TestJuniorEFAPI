<template>
<div>
    detail lead page ID:{{idIR}}
</div>
</template>

<script>
export default {
  data() {
    return {
        idIR:this.$route.params.id
    };
  },
};
</script>