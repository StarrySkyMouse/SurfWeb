<template>
  <div class="map-card" @click="openMap()">
    <div class="card-image" :style="{ backgroundImage: `url(${mapInfo.img})` }"></div>
    <div class="card-info">
      <span class="map-name">{{ mapInfo.mapName==null?mapInfo.name:mapInfo.mapName }}</span>
      <span class="map-difficulty">{{ mapInfo.difficulty }}</span>
    </div>
  </div>
</template>

<script>
export default {
  name: 'MapCard',
  props: {
    mapInfo: {
      type: Object,
      required: true,
      default: () => { }
    }
  },
  data() {
    return {
    }
  },
  methods: {
    openMap() {
      this.$emit('openMap', this.mapInfo)
    }
  }
}
</script>
<style scoped>

.map-card {
  width: 100%;
  max-width: 400px;
  margin: 16px auto;
  border-radius: 8px;
  overflow: hidden;
  background: #fff;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.08);
  transition: box-shadow 0.2s, transform 0.2s;
  cursor: pointer;
  display: flex;
  flex-direction: column;
}

.map-card:hover {
  transform: translateY(-3px) scale(1.03);
}

.card-image {
  width: 100%;
  aspect-ratio: 16 / 7;
  background-size: cover;
  background-position: center;
  border-radius: 8px 8px 0 0;
}

.card-info {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px 18px;
  background: #f9f9f9;
  border-radius: 0 0 18px 18px;
  font-size: 17px;
}

.map-name {
  font-weight: 600;
  color: #222;
  max-width: 70%;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.map-difficulty {
  border-radius: 10px;
  font-size: 14px;
  font-weight: 500;
  padding: 2px 10px;
}
</style>
