<template>
    <tr class="on-hover" @click="goToDetail(brand.id)"  >
        <th scope="row">{{brand.id}}</th>
        <td>{{brand.name}}</td>
        <td>{{brand.description}}</td>
    </tr>
</template>

<script>

export default {
    name:"BrandRow", 
    props:{
        brand:{
            type:Object,
            required:true
        }
    },
    methods:{
        goToDetail(brandId){
            this.$router.push("/brands/"+brandId)
        }
    }

}
</script>

<style>
.on-hover:hover{
        background-color: rgb(236, 234, 234);
    }
</style>