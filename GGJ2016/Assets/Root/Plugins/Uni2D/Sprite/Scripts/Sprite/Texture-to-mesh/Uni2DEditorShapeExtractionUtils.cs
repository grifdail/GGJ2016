#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public static class Uni2DEditorShapeExtractionUtils
{

	// Distinguish completely transparent pixels from significant pixel by
	// "binarizing" the texture via a bit array.
	// false if a pixel is not significant (= transparent), true otherwise
	public static BinarizedImage BinarizeTexture( Texture2D a_rTextureToFilter, float a_fAlphaCutOff )
	{
		if( a_rTextureToFilter == null )
		{
			return null;
		}

		// float to byte
		byte iAlphaCutOff32 = (byte) ( a_fAlphaCutOff * 255.0f );

		// Retrieve texture pixels (in 32bits format, faster)
		// Array is flattened / pixels laid left to right, bottom to top
		Color32[ ] oTexturePixels = a_rTextureToFilter.GetPixels32( );
		int iPixelCount = oTexturePixels.Length;

		// Create (padded) sprite shape pixels array (bit array)
		BinarizedImage oBinarizedTexture = new BinarizedImage( a_rTextureToFilter.width, a_rTextureToFilter.height, false );

		// Parse all pixels
		for( int iPixelIndex = 0; iPixelIndex < iPixelCount; ++iPixelIndex )
		{
			Color32 f4Pixel = oTexturePixels[ iPixelIndex ];
			oBinarizedTexture.SetUnpaddedPixel( iPixelIndex, ( f4Pixel.a >= iAlphaCutOff32 ), f4Pixel.a/255.0f );
		}

		// Fill 1px holes
		//oBinarizedTexture.FillInsulatedHoles( );
		return oBinarizedTexture;
	}


	// Double the width and height of a binarized image
	public static BinarizedImage ResizeImage( BinarizedImage a_rBinarizedImage )
	{
		int iImageHeight = a_rBinarizedImage.Height;
		int iImageWidth  = a_rBinarizedImage.Width;

		int iResizedImageHeight = 2 * iImageHeight;
		int iResizedImageWidth  = 2 * iImageWidth;

		BinarizedImage oResizedBinarizedImage = new BinarizedImage( iResizedImageWidth, iResizedImageHeight, false );

		// Upsampling
		// Copy original pixels to resized sprite pixels array
		for( int iResizedY = 0; iResizedY < iResizedImageHeight; ++iResizedY )
		{
			for( int iResizedX = 0; iResizedX < iResizedImageWidth; ++iResizedX )
			{
				// Euclidian div
				int iOriginalX = iResizedX / 2;
				int iOriginalY = iResizedY / 2;
				int iOriginalIndex = a_rBinarizedImage.Coords2PixelIndex( iOriginalX, iOriginalY );
				int iResizedIndex = oResizedBinarizedImage.Coords2PixelIndex( iResizedX, iResizedY );

				// Pixel copy
				oResizedBinarizedImage[ iResizedIndex ] = a_rBinarizedImage[ iOriginalIndex ];
			}
		}

		return oResizedBinarizedImage;
	}
	
	public static BinarizedImage DownScaleImage(BinarizedImage a_rBinarizedImage, int a_iHorizontalSubDivs, int a_iVerticalSubDivs)
	{
		int iImageWidth  = a_rBinarizedImage.Width;
		int iImageHeight = a_rBinarizedImage.Height;

		int iGridSubDivPerRow    = ( a_iVerticalSubDivs + 1 );
		int iGridSubDivPerColumn = ( a_iHorizontalSubDivs + 1 );
		int iGridSubDivCount     = iGridSubDivPerRow * iGridSubDivPerColumn;
		
		// Grid div dimensions
		float fGridSubDivPixelWidth  = ( (float) iImageWidth  ) / (float) iGridSubDivPerRow;
		float fGridSubDivPixelHeight = ( (float) iImageHeight ) / (float) iGridSubDivPerColumn;
		
		BinarizedImage oResizedBinarizedImage = new BinarizedImage( iGridSubDivPerRow, iGridSubDivPerColumn, false );
		// Iterate grid divs
		for( int iGridSubDivIndex = 0; iGridSubDivIndex < iGridSubDivCount; ++iGridSubDivIndex )
		{
			// ( X, Y ) grid div pos from grid div index (in grid div space)
			int iGridSubDivX = iGridSubDivIndex % iGridSubDivPerRow;
			int iGridSubDivY = iGridSubDivIndex / iGridSubDivPerRow;
			
			// Compute the pixel bounds for this subdivision
			int iStartX = Mathf.FloorToInt(iGridSubDivX * fGridSubDivPixelWidth);
			int iStartY = Mathf.FloorToInt(iGridSubDivY * fGridSubDivPixelHeight);
			
			int iEndX = Mathf.CeilToInt((iGridSubDivX + 1) * fGridSubDivPixelWidth);
			int iEndY = Mathf.CeilToInt((iGridSubDivY + 1) * fGridSubDivPixelHeight);
			
			int iWidth = iEndX - iStartX;
			int iHeight = iEndY - iStartY;
			
			// Set grid sub div as empty while no significant pixel found
			bool bEmptyGridSubDiv = true;
			float fGridSubDivAlphaMean = 0.0f;
			for( int iY = 0; iY < iHeight; ++iY )
			{
				for( int iX = 0; iX < iWidth; ++iX )
				{
					int iPixelIndex = a_rBinarizedImage.Coords2PixelIndex( iX + iStartX, iY + iStartY );

					// At least one pixel is significant => need to create the whole grid div
					if( a_rBinarizedImage[ iPixelIndex ] )
					{
						bEmptyGridSubDiv = false;
					}
					fGridSubDivAlphaMean += a_rBinarizedImage.GetAlphaValue(iPixelIndex);
				}
			}
			fGridSubDivAlphaMean /= iHeight * iWidth;
			
			int iResizedIndex = oResizedBinarizedImage.Coords2PixelIndex( iGridSubDivX, iGridSubDivY );
			oResizedBinarizedImage.SetInternalPixel(iResizedIndex, !bEmptyGridSubDiv, fGridSubDivAlphaMean);
		}
		
		return oResizedBinarizedImage;
	}
}
#endif