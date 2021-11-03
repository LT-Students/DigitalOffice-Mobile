package com.lt.digitaloffice

import android.content.Context
import android.os.Bundle
import android.util.Log
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.material.Button
import androidx.compose.material.MaterialTheme
import androidx.compose.material.Surface
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import com.lt.digitaloffice.models.auth.LoginResponse
import com.lt.digitaloffice.ui.theme.DigitalOfficeTheme
import kotlinx.serialization.decodeFromString
import kotlinx.serialization.json.Json
import org.chromium.net.*
import java.nio.ByteBuffer
import java.nio.charset.StandardCharsets
import java.util.concurrent.Executor
import java.util.concurrent.Executors

const val AUTH_URL = "https://auth.dev.ltdo.xyz/auth/login"

private const val TAG = "MyUrlRequestCallback"

class MyUrlRequestCallback : UrlRequest.Callback() {
  override fun onRedirectReceived(request: UrlRequest?, info: UrlResponseInfo?, newLocationUrl: String?) {
    Log.i(TAG, "onRedirectReceived method called.")
    // You should call the request.followRedirect() method to continue
    // processing the request.
    request?.followRedirect()
  }

  override fun onResponseStarted(request: UrlRequest?, info: UrlResponseInfo?) {
    Log.i(TAG, "onResponseStarted method called.")
    // You should call the request.read() method before the request can be
    // further processed. The following instruction provides a ByteBuffer object
    // with a capacity of 102400 bytes to the read() method.
    request?.read(ByteBuffer.allocateDirect(102400))
  }

  override fun onReadCompleted(request: UrlRequest?, info: UrlResponseInfo?, byteBuffer: ByteBuffer?) {
    Log.i(TAG, "onReadCompleted method called.")

    var responseBodyJson: String = ""

    byteBuffer?.flip()
    byteBuffer?.let {
      val byteArray = ByteArray(it.remaining())
      it.get(byteArray)
      responseBodyJson = String(byteArray, StandardCharsets.UTF_8)
    }.apply {
      Log.d(TAG, "$this")
    }

    val loginResponse = Json.decodeFromString<LoginResponse>(responseBodyJson)

    byteBuffer?.clear()

    // You should keep reading the request until there's no more data.
    request?.read(ByteBuffer.allocateDirect(102400))
  }

  override fun onSucceeded(request: UrlRequest?, info: UrlResponseInfo?) {
    Log.i(TAG, "onSucceeded method called.")
  }

  override fun onFailed(request: UrlRequest?, info: UrlResponseInfo?, error: CronetException?) {
    TODO("Not yet implemented")
  }
}

private const val TAG2 = "MyUploadDataProvider"
//TODO replace username and passowrd "_user & _pass"
var string: String ="{\"loginData\":\"admin\",\"password\":\"ltstudents\"}"
val charset = StandardCharsets.UTF_8

class MyUploadDataProvider() : UploadDataProvider() {

  override fun getLength(): Long {
    val size:Long = string.length.toLong()
    Log.e(TAG2,"Length = "+size)
    return size
  }

  override fun rewind(uploadDataSink: UploadDataSink?) {
    Log.e(TAG2,"REWIND IS CALLED")
    uploadDataSink!!.onRewindSucceeded()
  }

  override fun read(uploadDataSink: UploadDataSink?, byteBuffer: ByteBuffer?) {
    Log.e(TAG2,"READ IS CALLED")
    byteBuffer!!.put(string.toByteArray(charset))
    //byteBuffer.rewind()
    //For chunked uploads, true if this is the final read. It must be false for non-chunked uploads.
    uploadDataSink!!.onReadSucceeded(false)
  }
}

class MainActivity : ComponentActivity() {
  override fun onCreate(savedInstanceState: Bundle?) {
    super.onCreate(savedInstanceState)
    setContent {
      DigitalOfficeTheme {
        // A surface container using the 'background' color from the theme
        Surface(color = MaterialTheme.colors.background) {
          Greeting("Android", this)
        }
      }
    }
  }
}

@Composable
fun Greeting(name: String, context: Context) {
  Text(text = "Hello $name!")
  Button(onClick = { SendRequest(context) }) {

  }
}

fun SendRequest(context: Context) {
  val builder = CronetEngine.Builder(context)
  val cronetEngine: CronetEngine = builder
    .enableHttp2(true)
    .build()
  val executor: Executor = Executors.newSingleThreadExecutor()
  val requestBuilder = cronetEngine.newUrlRequestBuilder(
    AUTH_URL,
    MyUrlRequestCallback(),
    executor)
  requestBuilder.addHeader("Content-Type", "application/json; charset=UTF-8")
  requestBuilder.setHttpMethod("POST")
  requestBuilder.setUploadDataProvider(MyUploadDataProvider(), executor)
  val request: UrlRequest = requestBuilder.build()
  request.start()
}
