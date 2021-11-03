package com.lt.digitaloffice.models.auth

import kotlinx.serialization.Serializable

@Serializable
class LoginResponse (
  val userId: String,
  val accessToken: String,
  val refreshToken: String,
  val accessTokenExpiresIn: Long,
  val refreshTokenExpiresIn: Long)

