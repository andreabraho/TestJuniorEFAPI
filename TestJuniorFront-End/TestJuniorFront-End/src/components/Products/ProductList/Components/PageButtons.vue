<template>
  <div class="mt-5">
        <button type="button" 
                  class="ml-1 me-1" 
                  @click="previousPage()"
                  :class="[page==1?'disabled btn btn-outline-secondary':'btn btn-outline-primary']">Indietro</button>
        <button class="btn btn-outline-primary me-1"
                v-for="num in pages" 
                :key="num"
                @click="changePage(num)"
                :class="[num==page?'active':'']"
                >{{num}}</button>
        <button type="button" 
                class="ml-1" 
                @click="nextPage()"
                :class="[page==maxPages?'disabled btn btn-outline-secondary':'btn btn-outline-primary']">Avanti</button>
  </div>

</template>

<script>
export default {
    data(){
        return {
            pages:[]
        }
    },
    props:{
        page:{
            type:Number,
            required:true
        },
        maxPages:{
            type:Number,
            required:true
        }
    },
    methods:{
        selectPages(){
            this.pages=[]

            if(this.maxPages<=5){
                for(let i=1;i<=this.maxPages;i++)
                {
                    
                    this.pages.push(i)
                }
            }else{
                if(this.page<=3)
                    this.pages=[1,2,3,4,5]
                else
                    if(this.page>=this.maxPages-3)
                        this.pages=[this.maxPages-5,this.maxPages-4,this.maxPages-3,this.maxPages-2,this.maxPages-1]
                    else{
                        this.pages=[this.page-2,this.page-1,this.page,this.page+1,this.page+2]
                    }
            }
            

        },
        changePage(num){
            this.page=num
            this.$emit("changePage",this.page)
            this.selectPages()
        },
        async nextPage() {
            if (this.page < this.maxPages) {
                this.page = this.page + 1;
                this.$emit("changePage",this.page)
                this.selectPages()
            }

        },
        async previousPage() {

            if (this.page > 1) {
                this.page = this.page - 1;
                this.$emit("changePage",this.page)
                this.selectPages()

            }

        },
    },
    mounted(){
        this.selectPages()
    }
}
</script>

<style>

</style>