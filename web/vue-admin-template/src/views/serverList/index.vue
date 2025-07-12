<template>
  <div class="app-container">
    <div class="card">
      <div v-if="serverInfo.mapInfo == null" class="img"
        style="display: flex; justify-content: center;align-items: center;">
        网站暂未收录地图信息
      </div>
      <img v-else class="img" :src="serverInfo.mapInfo.img" alt="未收录地图图片" @click="openMap()" style="cursor: pointer;" />
      <div class="info">
        <div class="info-item">
          <span>地图:<span class="map-name" :title="serverInfo.map">{{ serverInfo.map }}</span></span>
        </div>
        <div class="info-item">
          <span>难度:{{ serverInfo.mapInfo == null ? '' : serverInfo.mapInfo.difficulty }}</span>
        </div>
        <div class="info-item">
          <span>玩家: <el-link type="primary" @click="dialogVisible = true">{{ serverInfo.playerInfos.length }}/{{
            serverInfo.maxPlayers }}</el-link></span>
        </div>
        <div class="info-item-button">
          <button onclick="window.location.href = 'steam://connect/124.223.198.48:27070'">加入</button>
        </div>
      </div>
    </div>
    <el-dialog title="查看玩家" :visible.sync="dialogVisible" width="30%">
      <el-table :data="serverInfo.playerInfos" borderfithighlight-current-row>
        <el-table-column align="center" label="#" width="120">
          <template slot-scope="scope">
            {{ scope.$index + 1 }}
          </template>
        </el-table-column>
        <el-table-column align="center" label="玩家">
          <template slot-scope="scope">
            <el-link type="primary">{{ scope.row.name }}</el-link>
          </template>
        </el-table-column>
        <el-table-column align="center" label="在线时长">
          <template slot-scope="scope">
            <span>{{ scope.row.duration }}</span>
          </template>
        </el-table-column>
      </el-table>
    </el-dialog>
  </div>
</template>
<script>

import { getServerInfo } from '@/api/serverList'

export default {
  data() {
    return {
      intervalId: null,
      dialogVisible: false,
      serverInfo: {}
    }
  },
  created() {
    this.loadData()
  },
  mounted() {
  },
  deactivated() {
    if (this.intervalId) {
      clearInterval(this.intervalId)
    }
  },
  beforeDestroy() {
    if (this.intervalId) {
      clearInterval(this.intervalId)
    }
  },
  methods: {
    openMap() {
      if (this.serverInfo.mapInfo) {
        this.$router.push({
          path: `/maps/detail/${this.serverInfo.mapInfo.id}`,
          query: { tagName: this.serverInfo.mapInfo.name }
        })
      }
    },
    fetchData() {
      getServerInfo().then(response => {
        this.serverInfo = response.data
      })
    },
    loadData() {
      if (this.intervalId) {
        clearInterval(this.intervalId)
      }
      this.fetchData()
      this.intervalId = setInterval(this.fetchData, 10000)
    }
  }
}
</script>
<style lang="scss" scoped>
.app-container {
  display: flex;
  align-items: center;
  min-height: 300px;
  font-family: 'Segoe UI', 'PingFang SC', 'Microsoft YaHei', Arial, sans-serif;
}

.card {
  display: flex;
  width: 550px;
  height: 180px;
  border-radius: 6px;
  background: #fff;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.08);
  overflow: hidden;
}

.img {
  flex: 3;
  height: 100%;
  object-fit: cover;
  border-top-left-radius: 12px;
  border-bottom-left-radius: 12px;
  /* 去掉原有的伪元素 */
}

.info {
  flex: 2;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  padding: 24px 20px 24px 20px;
}

.info-item {
  margin-bottom: 12px;
  font-size: 16px;
  color: #222;
  font-weight: 500;
  letter-spacing: 0.5px;
}

.info-item span {
  color: #303133;
  font-weight: 600;
}

.info-item-button {
  display: flex;
  justify-content: flex-end;
  align-items: flex-end;
}

.info-item-button button {
  border: none;
  background: #3d3f48;
  color: #fff;
  padding: 6px 20px;
  font-size: 16px;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 500;
  box-shadow: 0 1px 4px rgba(64, 158, 255, 0.08);
  transition: background 0.2s, box-shadow 0.2s;
}

.info-item-button button:hover {
  background: #6a6e81;
  box-shadow: 0 2px 8px rgba(64, 158, 255, 0.15);
}

.map-name {
  max-width: 160px;
  /* 限制最大宽度，可按需求调整 */
  overflow: hidden;
  /* 溢出隐藏 */
  text-overflow: ellipsis;
  /* 超出部分显示省略号 */
  white-space: nowrap;
  /* 不换行 */
  display: inline-block;
  /* 必须设置为inline-block或block */
  vertical-align: middle;
  /* 和文本对齐 */
}
</style>
