<template>
  <div class="tabs-wrapper">
    <div class="tabs-header">
      <div v-for="(item, idx) in tabs" :key="idx" class="tab-item" :class="{ active: idx === tabActive }"
        @click="showTab(idx)">
        {{ item }}
      </div>
    </div>
    <div class="tabs-content" id="tab-content">
      <slot></slot>
    </div>
  </div>
</template>

<script>
export default {
  name: 'Tabs',
  props: {
    tabs: {
      type: Array,
      required: true,
      default: () => []
    }
  },
  data() {
    return {
      tabActive: 0
    }
  },
  methods: {
    showTab(idx) {
      this.tabActive = idx
      this.$emit('tabChange', idx)
    }
  }
}
</script>
<style scoped>
.tabs-wrapper {
  background: #fff;
  border-radius: 4px;
  width: 100%;
  margin: 30px auto;
  margin-bottom: 0px;
  border: 1px solid #dcdfe6;
  box-shadow: 0 2px 4px 0 rgba(0, 0, 0, .12), 0 0 6px 0 rgba(0, 0, 0, .04);
  overflow: hidden;

}

.tabs-header {
  display: flex;
  background: #f7f9fa;
}

.tab-item {
  padding: 12px 24px;
  color: #888;
  cursor: pointer;
  font-size: 16px;
  background: transparent;
  border: none;
  outline: none;
  transition: color 0.2s;
  user-select: none;
}

.tab-item.active {
  color: #3d3f48;
  background: #fff;
  font-weight: 800;
  border-bottom: 2px solid #3d3f48;
}

.tabs-content {
  min-height: 625px;
  padding: 24px 8px;
  font-size: 20px;
  color: #222;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}
</style>
