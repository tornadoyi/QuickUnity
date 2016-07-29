import os
import io
import sys
import hashlib
import getopt
import shutil
import zlib
import json
import codecs
import subprocess
import time  
import thread 
import threading 
import re

import yaml
from utils import *

def load_yml(filepath):
	f = open(filepath, "r")
	data = f.read()
	f.close()
	return yaml.load(data)

def main():
	
	workspace = get_project_path()
	publish_path = path_combine(workspace, "../publish")
	straming_asset_path = path_combine(workspace, "../StreamingAssets")
	configpath = path_combine(workspace, "config_asset_bundle.yml")
	config = load_yml(configpath)
	buildpath = path_combine(workspace, config["build_path"])
	asset_table_name = config["asset_table_name"]
	bundle_ext = config["asset_bundle_ext"]


	
	def publish_to(infos, srcpath, dstpath):
		copyfiledict = {}
		asset_table_path = path_combine(dstpath, asset_table_name)

		# check
		if not os.path.exists(dstpath): os.makedirs(dstpath)

		# copy bundles
		for d in infos:
			srcf = path_combine(srcpath, "%s.%s" % (d["name"], bundle_ext))
			dstf = path_combine(dstpath, "%s.%s.%s" % (d["name"], d["version"], bundle_ext))
			dst_dir = os.path.dirname(dstf)
			copyfiledict[dstf] = True

			if os.path.exists(dstf): continue
			if not os.path.exists(dst_dir): os.makedirs(dst_dir)
			shutil.copy(srcf, dstf)

		# generate asset table
		str_table = yaml.dump({'asset_bundles' : infos}, default_flow_style=False)
		f = open(asset_table_path, "w")
		f.write(str_table)
		f.close()
		copyfiledict[asset_table_path] = True


		# clear unused
		def clear(dirpath, filename):
			filepath = path_combine(dirpath, filename)
			if copyfiledict.has_key(filepath): return
			print("remove unused", filepath)
			os.remove(filepath)

		search_path(dstpath, None, None, clear)


	def publish(target):
		target_build_path = path_combine(buildpath, target)
		target_publish_path = path_combine(publish_path, target)
		target_straming_asset_path = path_combine(straming_asset_path, target)
		build_file = path_combine(target_build_path, asset_table_name)

		local_bundle_infos = []
		server_bundle_infos = []

		buildconfig = load_yml(build_file)
		asset_bundles = buildconfig["asset_bundles"]
		for d in asset_bundles:
			local = d["local"]
			del d["local"]

			if local:
				local_bundle_infos.append(d)
			else:
				server_bundle_infos.append(d)

		# publish server asset bundles
		publish_to(server_bundle_infos, target_build_path, target_publish_path)

		# publish streamming assets bundles
		publish_to(local_bundle_infos, target_build_path, target_straming_asset_path)


	publish("iOS")
	publish("Android")

	print("publish success")

main()