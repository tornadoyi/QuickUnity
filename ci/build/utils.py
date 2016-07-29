import os
import io
import sys
import hashlib
import getopt
import shutil
import zlib
import json
import codecs


g_logPath = None



def get_project_path(): return os.path.split( os.path.realpath( sys.argv[0] ) )[0]

def get_unity_bin_path(): return "/Applications/Unity/Unity.app/Contents/MacOS/Unity"
	
def format_linux_path_separator(path): return path.replace("\\", "/")

def path_combine(src, dst): return format_linux_path_separator( os.path.abspath( os.path.join(src, dst) ) )

def init_log_system(logpath):
	if logpath == None: return
	global g_logPath
	g_logPath = logpath
	if os.path.exists(g_logPath): shutil.rmtree(g_logPath)
	os.makedirs(g_logPath)
	
def get_editor_log_path(logname=None): 
	dftlogname = "editor.log"
	if logname == None: logname = dftlogname

	global g_logPath
	if g_logPath == None: return os.path.join(get_project_path(), logname)
	return os.path.join(g_logPath, logname) 

def get_build_log_path(logname=None): 
	dftlogname = "build.log"
	if logname == None: logname = dftlogname
	
	global g_logPath
	if g_logPath == None: return os.path.join(get_project_path(), logname)
	return os.path.join(g_logPath, logname) 


def log(logstr):
	print(logstr)
	logfilePath = get_build_log_path()
	try:
		f = file(logfilePath, 'a') 
		f.write(logstr + "\n")
		f.close()
	except Exception, e:
		print(e)
		return
	
def print_build_log_content():
	logfilePath = get_build_log_path()
	try:
		f = file(logfilePath, 'r')
		content = f.read()
		f.close()
		print("\n\n\n")
		print("=========== Build Log ===========")
		print(content)
		print("=================================")
	except Exception, e:
		print(e)
		return

def print_editor_log_content():
	logfilePath = get_editor_log_path()
	try:
		f = file(logfilePath, 'r')
		content = f.read()
		f.close()
		print("\n\n\n")
		print("=========== Editor Log ===========")
		print(content)
		print("=================================")
	except Exception, e:
		print(e)
		return


def do_command(cmd):
	log(cmd)
	os.system(cmd)


def search_path(workspace, usefilter, unusedfilter, process):
	# search files
	for dirpath, dirnames, filenames in os.walk(workspace):
		for filename in filenames:
			splits = os.path.splitext(filename)
			ext = splits[len(splits)-1]
			if usefilter != None and usefilter.find(ext) < 0: continue
			if unusedfilter != None and usefilter.find(ext) >= 0: continue
			process(dirpath, filename)


def copy_files(src, dst):
	if not os.path.isdir(src): print("error: %s is not path" % (src)); return
	if not os.path.isdir(dst): print("error: %s is not path" % (dst)); return

	for f in os.listdir(src): 
		srcf = os.path.join(src, f)
		dstf = os.path.join(dst, f)

		# file then copy to dst
		if os.path.isfile(srcf):
			shutil.copy(srcf, dstf)
		else:
			#if not os.path.exists(dstf): os.makedirs(dstf) 
			#copy_files(srcf, dstf)
			if os.path.exists(dstf): shutil.rmtree(dstf) 
			shutil.copytree(srcf, dstf)


def copy_path(src, dst, rename = None):
	name = rename
	if rename == None: name = os.path.basename(src)
	shutil.copytree(src, os.path.join(dst, name))

def compress(infile, dst, level=9):
	infile = open(infile, 'rb')
	dst = open(dst, 'wb')
	compress = zlib.compressobj(level)
	data = infile.read(1024)
	while data:
		dst.write(compress.compress(data))
		data = infile.read(1024)
	dst.write(compress.flush())

def md5(filepath):
	m = hashlib.md5()  
	file = io.FileIO(filepath,'r')  
	bytes = file.read(1024)  
	while(bytes != b''):  
		m.update(bytes)  
		bytes = file.read(1024)   
	file.close()    
	md5value = m.hexdigest()
	return md5value






