<template>
  <div>ir list</div>
</template>

<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const IRRepository = MyRepositoryFactory.get("inforequests");
export default {
  data() {
    return {
      pageData: null,
      isLoadingIR: true,
    };
  },
  created() {},
  methods: {
    async load() {
      this.isLoadingIR = true;
      const { data } = await ProductRepository.getPage();
      this.pageData = data;
      this.isLoadingIR = false;


    },
  },
};
</script>