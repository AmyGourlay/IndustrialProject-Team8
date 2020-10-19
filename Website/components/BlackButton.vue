<template>
    <section>
        <div class="Readybutton">
            <b-button type="is-black" size="is-large">{{ title }}</b-button>
        </div>
    </section>
</template>

<script>
export default {
  props: {
    title: {
      type: String,
      required: true
    }
  }
}
</script>

</script>

<style>
.is-black {
  background-color: black;
}
</style>