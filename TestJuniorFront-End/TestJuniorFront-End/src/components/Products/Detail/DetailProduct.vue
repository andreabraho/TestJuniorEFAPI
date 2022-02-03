<template>
    <div>
        detail prod ID: {{idPorduct}}
    </div>
</template>

<script>

export default ({
    data(){
        return {
            idPorduct:this.$route.params.id
        }
    }
})
</script>


