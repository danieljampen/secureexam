<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:draw="urn:oasis:names:tc:opendocument:xmlns:drawing:1.0" xmlns:text="urn:oasis:names:tc:opendocument:xmlns:text:1.0" xmlns:form="urn:oasis:names:tc:opendocument:xmlns:form:1.0">
  <xsl:output indent="yes" encoding="ISO-8859-1" method="xml" />

   <xsl:template match="question">
      <xsl:element name="question">
		<xsl:variable name="QUESTIONNR" select="@nr" />
		<legend>
			<xsl:value-of select="." />
		</legend>
        <xsl:for-each select="following-sibling::answer">
			<xsl:if test="@questionNr = $QUESTIONNR">
				<xsl:copy-of select="input" />
			</xsl:if>
		</xsl:for-each>
      </xsl:element>
  </xsl:template>
  
  <xsl:template match="/">
    <questions>
	  <xsl:apply-templates select=".//question" />
    </questions>
  </xsl:template>
</xsl:stylesheet>
